import './App.css'
import { Route, Routes, useNavigate, useLocation } from 'react-router-dom'
import HomePage from './pages/HomePage'
import LoginPage from './pages/LoginPage'
import RegistrationPage from './pages/RegistrationPage'
import NotFoundPage from './pages/NotFoundPage'
import Navbar from './components/Navbar'
import { logout } from './api/auth'

function App() {
  const navigate = useNavigate()
  const location = useLocation()

  const handleLogout = () => {
    logout();
    navigate('/login')
  }

  const hideNavbar = location.pathname === '/login' || location.pathname === '/register'

  return (
    <>
      {!hideNavbar && <Navbar onLogout={handleLogout} />}
      <Routes>
        <Route path='/' element={<HomePage />} />
        <Route path='/register' element={<RegistrationPage />} />
        <Route path='/login' element={<LoginPage />} />
        <Route path='*' element={<NotFoundPage />} />
      </Routes>
    </>
  )
}

export default App
