import { useEffect, useState } from 'react';


const TOKEN_KEY = 'noc_token'
const USER_KEY = 'noc_user'

export default function Login() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState(null)
  const [message, setMessage] = useState(null)
  const [token, setToken] = useState(null)

  useEffect(() => {
    // Keep session on page refresh by reading localStorage
    const existing = localStorage.getItem(TOKEN_KEY)
    if (existing) {
      setToken(existing)
      setMessage('Already logged in')
    }
  }, [])

  async function handleLogin(e) {
    e.preventDefault()
    setError(null)
    setMessage(null)

    try {
        const res = await fetch('https://localhost:7027/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ "username":username, "password":password }),
      })

      const data = await res.json().catch(() => null)
      if (!res.ok) {
        setError(data?.message || 'Login failed')
        return
      }

      // Save token and username to localStorage to persist session across refresh
      if (data?.token) {
        localStorage.setItem(TOKEN_KEY, data.token)
        localStorage.setItem(USER_KEY, data.username || username)
        setToken(data.token)
        setMessage(data.message || 'Login successful')
        setUsername('')
        setPassword('')
      } else {
        setError('Login response did not include a token')
      }
    } catch (ex) {
      setError('Network error')
    }
  }

  function handleLogout() {
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(USER_KEY)
    setToken(null)
    setMessage('Logged out')
  }

  return (
    <div style={{ maxWidth: 420, margin: '0 auto' }}>
      <h2>Login</h2>

      {token ? (
        <div className="Main-Container">
          <div>Logged in (token present)</div>
          <div style={{ wordBreak: 'break-all', marginTop: 8 }}>{token}</div>
          <div style={{ marginTop: 12 }}>
            <button onClick={handleLogout}>Logout</button>
          </div>
          {message && <div style={{ color: 'green', marginTop: 8 }}>{message}</div>}
        </div>
      ) : (
        <form onSubmit={handleLogin}>
          <div>
            <label>Username</label>
            <input value={username} onChange={e => setUsername(e.target.value)} required />
          </div>
          <div>
            <label>Password</label>
            <input type="password" value={password} onChange={e => setPassword(e.target.value)} required />
          </div>
          <div style={{ marginTop: 12 }}>
            <button type="submit">Login</button>
          </div>
          {error && <div style={{ color: 'crimson', marginTop: 8 }}>{error}</div>}
        </form>
      )}
    </div>
  )
}
