import React from "react";
import "./App.css";
import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Signup from "./pages/Signup";
import Search from "./pages/Search";
import Profile from "./pages/Profile";
import Messanger from "./pages/Messanger";
import Login from "./pages/Login";

function App() {
  return (
    <div className="App">
      <Routes>
        <Route exact path={"/"} element={<Home />} />
        <Route path={"/login"} element={<Login />} />
        <Route path={"/signup"} element={<Signup />} />
        <Route path={"/search"} element={<Search />} />
        <Route path={"/profile"} element={<Profile />} />
        <Route path={"/messanger"} element={<Messanger />} />
      </Routes>
    </div>
  );
}

export default App;
