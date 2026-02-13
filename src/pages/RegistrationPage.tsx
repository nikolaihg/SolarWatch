import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import InputForm from "../components/InputForm";
import { register } from "../api/auth";

function RegistrationPage() {
    const navigate = useNavigate();
    const [error, setError] = useState<string | null>(null);

    const handleRegister = async (userData: { email: string; password: string }) => {
        setError(null);
        try {
            await register(userData);
            navigate('/');
        } catch (error) {
            setError('Registration failed. Please try again.');
            console.error(error);
        }
    }

    return (
        <>
            <h1>Register an account here:</h1>
            {error && <div className="error-message">{error}</div>}
            <InputForm onSubmitSuccess={handleRegister} buttonLabel="Register" />
            <Link to="/login" style={{ color: '#646cff', textDecoration: 'underline' }}>
                Already have an account? Click here to login.
            </Link>
        </>
    );
}

export default RegistrationPage