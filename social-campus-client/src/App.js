import React from "react";
import "./App.css";
import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import SignUp from "./pages/signup/SignUp";
import Search from "./pages/Search";
import Profile from "./pages/profile/Profile";
import Messanger from "./pages/Messanger";
import SignIn from "./pages/signin/SignIn";
import Menu from "./pages/menu/Menu";
import Welcom from "./pages/welcom/Welcom";
import Footer from "./components/footer/Footer";
import Header from "./components/header/Header";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
  return (
    <div className="App">
      <Header />
      <div className="main-container">
        <Routes>
          <Route exact path={"/"} element={<Welcom />} />
          <Route path={"/home"} element={<Home />} />
          <Route path={"/signin"} element={<SignIn />} />
          <Route path={"/menu"} element={<Menu />} />
          <Route path={"/signup"} element={<SignUp />} />
          <Route path={"/search"} element={<Search />} />
          <Route path={"/profile"} element={<Profile />} />
          <Route path={"/messanger"} element={<Messanger />} />
        </Routes>
      </div>
      <Footer />
      <ToastContainer />
    </div>
  );
}

export default App;
