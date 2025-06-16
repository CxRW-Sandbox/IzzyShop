import React, { useState } from 'react';
import axios from 'axios';

function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [message, setMessage] = useState('');
  const [profile, setProfile] = useState(null);

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

  const viewProfile = async () => {
    try {
      const res = await axios.post('/api/auth/profile', { username });
      setProfile(res.data);
    } catch (err) {
      setMessage('Could not load profile');
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
      <button type="button" onClick={viewProfile}>View My Profile</button>
      {profile && (
        <div>
          <h3>Profile for {profile.username}</h3>
          <div>Bio: <span dangerouslySetInnerHTML={{ __html: profile.profileBio }} /></div>
          <div>Address: <span dangerouslySetInnerHTML={{ __html: profile.address }} /></div>
        </div>
      )}
      <div dangerouslySetInnerHTML={{ __html: message }} />
    </div>
  );
}

export default Login; 