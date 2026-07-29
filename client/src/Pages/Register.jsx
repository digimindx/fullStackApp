import { useState } from 'react'

export default function Register() {
  const [username, setUsername] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [message, setMessage] = useState(null)
  const [error, setError] = useState(null)

  async function handleSubmit(e) {
    e.preventDefault()
    setMessage(null)
    setError(null)

    if (password !== confirmPassword) {
      setError('Passwords do not match')
      return
    }

      const payload = { "FirstName": "", "MiddleName": "","LastName":"","username":username, "":email, password }

    try {
      const res = await fetch('/api/auth/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      })

      if (!res.ok) {
        const data = await res.json().catch(() => null)
        setError(data?.message || 'Registration failed')
        return
      }

      const data = await res.json().catch(() => null)
      setMessage(data?.message || 'Registration successful')
      setUsername('')
      setEmail('')
      setPassword('')
      setConfirmPassword('')
    } catch (ex) {
      setError('Error: ' + ex.message)
    }
  }

  return (
    <div style={{ maxWidth: 420, margin: '0 auto' }}>
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Username</label>
          <input value={username} onChange={e => setUsername(e.target.value)} required />
        </div>
        <div>
          <label>Email</label>
          <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
        </div>
        <div>
          <label>Password</label>
          <input type="password" value={password} onChange={e => setPassword(e.target.value)} required />
        </div>
        <div>
          <label>Confirm Password</label>
          <input type="password" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} required />
        </div>
        <div style={{ marginTop: 12 }}>
          <button type="submit">Register</button>
        </div>
      </form>

      {message && <div style={{ color: 'green', marginTop: 8 }}>{message}</div>}
      {error && <div style={{ color: 'crimson', marginTop: 8 }}>{error}</div>}
    </div>
  )
}
