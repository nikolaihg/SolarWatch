import { Link, useNavigate } from "react-router-dom";
import InputForm from "../components/InputForm";
import { login } from "../api/auth";

function LoginPage() {
    const navigate = useNavigate();

    const handleLogin = async (credentials: { email: string; password: string }) => {
        try {
            await login(credentials);
            navigate('/');
        } catch (error) {
            alert('Login failed');
            console.error(error);
        }
    }

    return (
        <>
            <h1>Please log in:</h1>
            <InputForm onSubmitSuccess={handleLogin} />
            <p> Dont have a account?</p>
            <Link to="/register" style={{ color: '#646cff', textDecoration: 'underline' }}>
                Click here to register.
            </Link>
        </>
    );
}

export default LoginPage