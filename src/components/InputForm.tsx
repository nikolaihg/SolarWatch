import { useState } from "react";

type InputFormProps = {
  onSubmitSuccess?: (data: { email: string; password: string }) => void;
};

function InputForm({ onSubmitSuccess }: InputFormProps) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (event: React.SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    // min 8 chars, lowercase, uppercase, number, symbol
    const passwordRegex =
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()[\]{}\-_=+\\|;:'",.<>/?]).{8,}$/;

    const validations = [
      { condition: !email.trim(), message: "Email is required" },
      {
        condition: email && !emailRegex.test(email),
        message: "Email must be a valid email address",
      },
      { condition: !password, message: "Password is required" },
      {
        condition: password && !passwordRegex.test(password),
        message:
          "Password must be ≥8 characters and include uppercase, lowercase, number, and symbol",
      },
    ];

    for (const validation of validations) {
      if (validation.condition) {
        alert(validation.message);
        return;
      }
    }

    const formData = { email, password };

    console.log("Login success:", formData);
    onSubmitSuccess?.(formData);
  };

  return (
    <div className="card form-card">
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Email:</label>
          <input
            className="email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>
        <div className="form-group">
          <label>Password:</label>
          <input
            className="password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <button type="submit">Login</button>
      </form>
      
    </div>
  );
}

export default InputForm;
