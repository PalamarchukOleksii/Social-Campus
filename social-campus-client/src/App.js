import React from "react";
import "./App.css";
import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import SignUp from "./pages/signup/SignUp";
import Search from "./pages/Search";
import Profile from "./pages/Profile";
import Messanger from "./pages/Messanger";
import SignIn from "./pages/signin/SignIn";
import Welcom from "./pages/welcom/Welcom";

function App() {
  return (
    <div className="App">
      <Routes>
        <Route exact path={"/"} element={<Welcom />} />
        <Route path={"/home"} element={<Home />} />
        <Route path={"/signin"} element={<SignIn />} />
        <Route path={"/signup"} element={<SignUp />} />
        <Route path={"/search"} element={<Search />} />
        <Route path={"/profile"} element={<Profile />} />
        <Route path={"/messanger"} element={<Messanger />} />
      </Routes>
    </div>
  );
}

export default App;
