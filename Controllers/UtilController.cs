using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data;
#if NET7_0_OR_GREATER
using Microsoft.CodeAnalysis.CSharp.Scripting;
#endif
using System.Threading.Tasks;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilController : ControllerBase
    {
        // Command Injection: Ping
        [HttpPost("ping")]
        public IActionResult Ping([FromBody] string host)
        {
            // Vulnerable: User input passed directly to shell
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C ping {host}",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            var proc = Process.Start(psi);
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return Ok(output);
        }

        // Command Injection: Echo
        [HttpPost("echo")]
        public IActionResult Echo([FromBody] string text)
        {
            // Vulnerable: User input passed directly to shell
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C echo {text}",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            var proc = Process.Start(psi);
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return Ok(output);
        }

        // Code Injection: Evaluate C# Expression
        [HttpPost("eval")]
        public async Task<IActionResult> Eval([FromBody] string code)
        {
#if NET7_0_OR_GREATER
            // Vulnerable: Evaluates user-supplied C# code
            var result = await CSharpScript.EvaluateAsync(code);
            return Ok(result?.ToString());
#else
            return Ok("Not supported in this build.");
#endif
        }

        // Code Injection: Evaluate Math Expression
        [HttpPost("calc")]
        public IActionResult Calc([FromBody] string expr)
        {
            // Vulnerable: Evaluates user-supplied expression
            var dt = new DataTable();
            var result = dt.Compute(expr, "");
            return Ok(result.ToString());
        }

        // Server-Side Template Injection (SSTI)
        [HttpPost("ssti")]
        public IActionResult SSTI([FromBody] TemplateRequest req)
        {
            // Vulnerable: Naive template rendering
            var template = req.Template;
            var result = template.Replace("{{username}}", req.Username)
                                 .Replace("{{admin}}", req.IsAdmin.ToString());
            // Simulate code execution by allowing {{code}} to be replaced with user input
            result = result.Replace("{{code}}", req.Code);
            return Ok(result);
        }

        public class TemplateRequest { public string Template { get; set; } public string Username { get; set; } public bool IsAdmin { get; set; } public string Code { get; set; } }
    }
} 