import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import InputForm from "../components/InputForm";
import { login } from "../api/auth";

function LoginPage() {
    const navigate = useNavigate();
    const [error, setError] = useState<string | null>(null);

    const handleLogin = async (credentials: { email: string; password: string }) => {
        setError(null);
        try {
            await login(credentials);
            navigate('/');
        } catch (error) {
            setError('Login failed. Please check your credentials.');
            console.error(error);
        }
    }

    return (
        <>
            <h1>Please log in:</h1>
            {error && <div className="error-message">{error}</div>}
            <InputForm onSubmitSuccess={handleLogin} buttonLabel="Login" validateComplexity={false} />
            <p> Dont have a account?</p>
            <Link to="/register" style={{ color: '#646cff', textDecoration: 'underline' }}>
                Click here to register.
            </Link>
        </>
    );
}

export default LoginPage