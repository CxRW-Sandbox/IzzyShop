import React, { useState } from 'react';
import axios from 'axios';

function Register() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const [bio, setBio] = useState('');

  const handleRegister = async (e) => {
    e.preventDefault();
    // Vulnerable: No password complexity check
    if (!username || !password) {
      setMessage('Username and password required.');
      return;
    }
    try {
      await axios.post('/api/auth/register', { username, password, email, profileBio: bio });
      setMessage('Registration successful!');
    } catch (err) {
      // Vulnerable: XSS in error message
      setMessage('Registration failed: ' + err.response?.data?.message || err.message);
    }
  };

  return (
    <div>
      <h2>Register</h2>
      <form onSubmit={handleRegister}>
        <input value={username} onChange={e => setUsername(e.target.value)} placeholder="Username" />
        <input value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" type="password" />
        <input value={email} onChange={e => setEmail(e.target.value)} placeholder="Email" />
        <textarea value={bio} onChange={e => setBio(e.target.value)} placeholder="Profile Bio (XSS demo)" />
        <button type="submit">Register</button>
      </form>
      <div dangerouslySetInnerHTML={{ __html: message }} />
    </div>
  );
}

export default Register; 