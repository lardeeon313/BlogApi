import { SyntheticEvent, useState } from "react";
import { Navigate } from "react-router-dom";
import axios from "axios";
import "tailwindcss/tailwind.css"



axios.defaults.withCredentials = true;

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [redirect, setRedirect] = useState(false);

  const submitt = async (e: SyntheticEvent) => {
    e.preventDefault();
    
    try {
      const response = await axios.post(
        "http://localhost:5238/api/Auth/login",
        { email, password },
        { withCredentials: true }
      );

      console.log("Login Exitoso:", response);
      
      if (response.status === 200) {
        setRedirect(true);
      }
    } catch (error) {
      console.error("Error en el login:", error);
    }
  };

  if (redirect) {
    return <Navigate to={"/"} />;
  }

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <div className="bg-white p-8 rounded-lg shadow-lg w-96">
        <h2 className="text-2xl font-bold mb-4 text-center text-gray-800">Iniciar Sesión</h2>
        <form onSubmit={submitt} className="space-y-4">
          <div>
            <label htmlFor="email" className="block text-sm font-medium text-gray-700">
              Correo Electrónico
            </label>
            <input
              type="email"
              id="email"
              className="mt-1 p-2 w-full border rounded focus:ring focus:ring-blue-300"
              placeholder="name@example.com"
              required
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>

          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700">
              Contraseña
            </label>
            <input
              type="password"
              id="password"
              className="mt-1 p-2 w-full border rounded focus:ring focus:ring-blue-300"
              placeholder="••••••••"
              required
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>

          <button
            type="submit"
            className="w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600 transition"
          >
            Iniciar Sesión
          </button>
          <p className="mt-3 text-gray-500 text-center">&copy; 2025</p>
        </form>
      </div>
    </div>
  );
};

export default Login;
