import './App.css'
import { Route, Routes, useNavigate } from 'react-router-dom'
import HomePage from './pages/HomePage'
import LoginPage from './pages/LoginPage'
import RegistrationPage from './pages/RegistrationPage'
import NotFoundPage from './pages/NotFoundPage'
import Navbar from './components/Navbar/Navbar'
import ProtectedRoute from './components/ProtectedRoute'
import { logout } from './api/auth'

function App() {
  const navigate = useNavigate()

  const handleLogout = () => {
    logout();
    navigate('/login')
  }

  return (
    <Routes>
      <Route element={<Navbar onLogout={handleLogout} />}>
        <Route path='/' element={<ProtectedRoute><HomePage /></ProtectedRoute>} />
      </Route>
      <Route path='/login' element={<LoginPage />} />
      <Route path='/register' element={<RegistrationPage />} />
      <Route path='*' element={<NotFoundPage />} />
    </Routes>
  )
}

export default App
