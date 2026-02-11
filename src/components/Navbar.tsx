import { useNavigate } from 'react-router-dom'
import './Navbar.css'

interface NavbarProps {
  onLogout: () => void
}

function Navbar({ onLogout }: NavbarProps) {
  const navigate = useNavigate()

  return (
    <nav className="navbar">
      <button className="nav-button home-button" onClick={() => navigate('/')}>
        Home
      </button>
      <button className="nav-button logout-button" onClick={onLogout}>
        Logout
      </button>
    </nav>
  )
}

export default Navbar
