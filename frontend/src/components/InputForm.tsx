import { useState } from "react";

type InputFormProps = {
  buttonLabel: string;
  onSubmitSuccess?: (data: { email: string; password: string }) => void;
  validateComplexity?: boolean;
};

function InputForm({ buttonLabel, onSubmitSuccess, validateComplexity = true }: InputFormProps) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = (event: React.SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError(null);

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    // min 8 chars, lowercase, uppercase, number, symbol
    const passwordRegex =
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()[\]{}\-_=+\\|;:'",.<>/?]).{8,}$/;

    const validations = [
      { condition: !email.trim(), message: "Email is required" },
      {
        condition: validateComplexity && email && !emailRegex.test(email),
        message: "Email must be a valid email address",
      },
      { condition: !password, message: "Password is required" },
      {
        condition: validateComplexity && password && !passwordRegex.test(password),
        message:
          "Password must be ≥8 characters and include uppercase, lowercase, number, and symbol",
      },
    ];

    for (const validation of validations) {
      if (validation.condition) {
        setError(validation.message);
        return;
      }
    }

    const formData = { email, password };

    console.log(`${buttonLabel} success:`, formData);
    onSubmitSuccess?.(formData);
  };

  return (
    <div className="bg-white rounded-xl shadow-xl p-8 max-w-sm w-full border border-gray-100">
      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="space-y-2">
          <label className="text-sm font-medium text-gray-700">Email</label>
          <input
            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="john@example.com"
          />
        </div>
        <div className="space-y-2">
          <label className="text-sm font-medium text-gray-700">Password</label>
          <input
            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="••••••••"
          />
        </div>
        <button 
          type="submit"
          className="w-full bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md transition-colors"
        >
          {buttonLabel}
        </button>
      </form>
      {error && (
        <div className="mt-4 p-3 bg-red-50 text-red-700 text-sm rounded-md border border-red-200">
          {error}
        </div>
      )}
    </div>
  );
}

export default InputForm;
