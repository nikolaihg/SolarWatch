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
        <div className="flex flex-col items-center justify-center min-h-[60vh]">
            <h1 className="text-3xl font-bold text-gray-800 mb-8">Create Account</h1>
            {error && <div className="mb-4 p-3 bg-red-100 text-red-800 rounded-lg max-w-sm w-full text-center">{error}</div>}
            <InputForm onSubmitSuccess={handleRegister} buttonLabel="Register" />
            <div className="mt-6 text-gray-600 flex gap-2">
                <span>Already have an account?</span>
                <Link to="/login" className="text-blue-600 hover:text-blue-800 font-medium hover:underline">
                    Login
                </Link>
            </div>
        </div>
    );
}

export default RegistrationPage