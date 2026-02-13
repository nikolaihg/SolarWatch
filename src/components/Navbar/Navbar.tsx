import { useEffect, useState } from 'react'
import { useNavigate, Outlet } from 'react-router-dom'
import { getToken } from '../../api/auth'
import './Navbar.css'

interface NavbarProps {
  onLogout: () => void
}

const decodeEmailFromToken = (token: string | null): string | null => {
  if (!token) return null

  try {
    const [, payload] = token.split('.')
    if (!payload) return null

    const base64 = payload.replace(/-/g, '+').replace(/_/g, '/')
    const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=')
    const json = atob(padded)
    const data = JSON.parse(json) as { email?: string }

    return data.email ?? null
  } catch (error) {
    console.error('Failed to decode token payload', error)
    return null
  }
}

function Navbar({ onLogout }: NavbarProps) {
  const navigate = useNavigate()
  const [email, setEmail] = useState<string | null>(null)

  useEffect(() => {
    const token = getToken()
    const decodedEmail = decodeEmailFromToken(token)
    setEmail(decodedEmail)
  }, [])

  return (
    <>
      <nav className="navbar">
        <div className="nav-left">
          <button className="nav-button home-button" onClick={() => navigate('/')}>
            Home
          </button>
        </div>
        <div className="nav-right">
          {email && <span className="user-email">{email}</span>}
          <button className="nav-button logout-button" onClick={onLogout}>
            Logout
          </button>
        </div>
      </nav>
      <Outlet />
    </>
  )
}

export default Navbar
