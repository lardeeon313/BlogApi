import { Link } from "react-router-dom";

const Nav = () => {
    return (
        <nav className="bg-gray-900 text-white p-4 shadow-md">
            <div className="container mx-auto flex justify-between items-center">
                <Link className="text-xl font-bold" to="/">Home</Link>

                <div className="space-x-4">
                    <Link className="hover:text-gray-300 transition" to="/login">Login</Link>
                    <Link className="hover:text-gray-300 transition" to="/register">Register</Link>
                </div>
            </div>
        </nav>
    );
};

export default Nav;
