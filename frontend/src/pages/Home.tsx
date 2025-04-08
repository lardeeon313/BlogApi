import React, { useState, useEffect } from 'react';
import axios from 'axios';
import "tailwindcss/tailwind.css";

axios.defaults.withCredentials = true;

const Home = () => {
    const [name, setName] = useState<string | null>(null);

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await fetch('http://localhost:5238/api/Auth/user', {
                    headers: { 'Content-Type': 'application/json' },
                    credentials: 'include',
                });

                if (!response.ok) {
                    throw new Error('Error al obtener usuario');
                }

                const content = await response.json();

                // 🔹 Ajustar la clave correcta del JSON
                setName(content.Data?.Email || 'Usuario');
            } catch (error) {
                console.error('Error al obtener el usuario:', error);
                setName('Usuario');
            }
        };

        fetchUser();
    }, []); // ✅ Agregamos dependencia vacía para que se ejecute solo una vez

    return (
        <div className="bg-gray-100 min-h-screen flex items-center justify-center">
            <div className="bg-purple-300 rounded-md p-10 flex justify-center items-center">
                <h1 className="text-xl font-bold">Bienvenido, {name}!</h1>
            </div>
        </div>
    );
};

export default Home;
