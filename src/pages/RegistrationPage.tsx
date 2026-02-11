import { Link } from "react-router-dom";
import InputForm from "../components/InputForm";

function RegistrationPage() {
    return (
        <>
            <h1>Register an account here:</h1>
            <InputForm />
            <p> Already have a account?</p>
            <Link to="/login" style={{ color: '#646cff', textDecoration: 'underline' }}>
                Click here to log in.
            </Link>
        </>
    );
}

export default RegistrationPage