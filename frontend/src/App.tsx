import './App.css'
import {BrowserRouter, Route, Routes} from "react-router-dom"
import Nav from './components/Nav';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import "./index.css";
import "tailwindcss/tailwind.css"





function App() {

  return (
    
    <>
      <div className='App'>
        <main>
          <BrowserRouter>
            <Nav/>
            <Routes>
            <Route path="/" element={<Home/>}/>
            <Route path="/login" element={<Login/>}/>
            <Route path="/register" element={<Register/>}/>
            </Routes>
          </BrowserRouter>
        </main>
      </div>
    </>
  
  )
}

export default App
