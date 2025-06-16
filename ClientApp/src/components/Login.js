import React, { useState } from 'react';
import axios from 'axios';

function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [message, setMessage] = useState('');

  const handleLogin = async (e) => {
    e.preventDefault();
    // Vulnerable: No input sanitization, reflected XSS in message
    if (!username || !password) {
      setMessage(`Missing username or password: ${window.location.search}`);
      return;
    }
    try {
      const res = await axios.post('/api/auth/login', { username, password });
      // Vulnerable: Store token in localStorage (no expiration, no secure flag)
      localStorage.setItem('token', res.data.token);
      setMessage('Login successful!');
    } catch (err) {
      setMessage('Login failed: ' + err.message);
    }
  };

  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <input value={username} onChange={e => setUsername(e.target.value)} placeholder="Username" />
        <input value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" type="password" />
        <button type="submit">Login</button>
      </form>
      <div dangerouslySetInnerHTML={{ __html: message }} />
    </div>
  );
}

export default Login; 