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
        <div className="flex flex-col items-center justify-center min-h-[60vh]">
            <h1 className="text-3xl font-bold text-gray-800 mb-8">Welcome Back</h1>
            {error && <div className="mb-4 p-3 bg-red-100 text-red-800 rounded-lg max-w-sm w-full text-center">{error}</div>}
            <InputForm onSubmitSuccess={handleLogin} buttonLabel="Login" validateComplexity={false} />
            <div className="mt-6 text-gray-600">
                <p>Don't have an account?</p>
                <Link to="/register" className="text-blue-600 hover:text-blue-800 font-medium hover:underline">
                    Register here
                </Link>
            </div>
        </div>
    );
}

export default LoginPage