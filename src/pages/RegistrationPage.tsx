import { useNavigate } from "react-router-dom";
import InputForm from "../components/InputForm";
import { register } from "../api/auth";

function RegistrationPage() {
    const navigate = useNavigate();

    const handleRegister = async (userData: { email: string; password: string }) => {
        try {
            await register(userData);
            navigate('/');
        } catch (error) {
            alert('Registration failed');
            console.error(error);
        }
    }

    return (
        <>
            <h1>Register an account here:</h1>
            <InputForm onSubmitSuccess={handleRegister} />
    
        </>
    );
}

export default RegistrationPage