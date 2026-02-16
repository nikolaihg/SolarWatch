import { useState } from 'react'
import { useNavigate, Outlet } from 'react-router-dom'
import { getToken } from '../../api/auth'


interface NavbarProps {
  onLogout: () => void
}

const decodeToken = (token: string | null): { email: string | null, role: string | null } => {
  if (!token) return { email: null, role: null }

  try {
    const [, payload] = token.split('.')
    if (!payload) return { email: null, role: null }

    const base64 = payload.replace(/-/g, '+').replace(/_/g, '/')
    const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=')
    const json = atob(padded)
    const data = JSON.parse(json)

    const email = data.email || data['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || null
    const role = data.role || data['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null

    return { email, role }
  } catch (error) {
    console.error('Failed to decode token payload', error)
    return { email: null, role: null }
  }
}

function Navbar({ onLogout }: NavbarProps) {
  const navigate = useNavigate()
  const [user] = useState<{ email: string | null, role: string | null }>(() => {
    const token = getToken()
    return decodeToken(token)
  })

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
              {user.email && (
                <div className="flex items-center gap-2">
                  <span className="text-sm text-gray-300 hidden md:inline">{user.email}</span>
                  {user.role && (
                    <span 
                      className={`px-2 py-0.5 rounded-full text-xs font-semibold ${
                        user.role === 'Admin' 
                          ? 'bg-purple-900 text-purple-200 border border-purple-700' 
                          : 'bg-emerald-900 text-emerald-200 border border-emerald-700'
                      }`}
                    >
                      {user.role}
                    </span>
                  )}
                </div>
              )}
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
