import { useState, useEffect } from 'react'
import "../Assets/css/App.css"
import Login from './Login'
import Register from './Register'

function App() {
  const [showRegister, setShowRegister] = useState(false)
  const [token, setToken] = useState(null)

  useEffect(() => {
    const existing = localStorage.getItem('noc_token')
    setToken(existing)
  }, [])

  return (
    <div className="Main-Container">
      
      <Login />

      <div style={{ marginTop: 16 }}>
        {showRegister ? (
          <div>
            <button onClick={() => setShowRegister(false)}>Back to Login</button>
            <div style={{ marginTop: 12 }}>
              <Register />
            </div>
          </div>
        ) : (
          <div>
            <span>Need a new account? </span>
            <button onClick={() => setShowRegister(true)}>Register</button>
          </div>
        )}
      </div>
    </div>
  )
}

export default App
