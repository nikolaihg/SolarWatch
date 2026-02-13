import { useEffect, useState } from 'react'
import { useNavigate, Outlet } from 'react-router-dom'
import { getToken } from '../../api/auth'


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
      <nav className="bg-slate-800 text-white shadow-lg">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between h-16 items-center">
            <div className="flex-shrink-0">
              <button 
                className="text-xl font-bold hover:text-blue-400 transition-colors" 
                onClick={() => navigate('/')}
              >
                SolarWatch
              </button>
            </div>
            <div className="flex items-center gap-4">
              {email && <span className="text-sm text-gray-300 hidden md:inline">{email}</span>}
              <button 
                className="bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded-md text-sm font-medium transition-colors cursor-pointer" 
                onClick={onLogout}
              >
                Logout
              </button>
            </div>
          </div>
        </div>
      </nav>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <Outlet />
      </div>
    </>
  )
}

export default Navbar
