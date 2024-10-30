
// menu/Menu.jsx
import React from "react";
import { useNavigate } from "react-router-dom";
import "./Menu.css"; 

function Menu() {
  const navigate = useNavigate();

  const goToProfile = () => {
    navigate("/profile");
  };

  return (
    <div className="menu">
      <h2>Menu</h2>
      <ul>
        <li><button onClick={goToProfile}>User</button></li> {}
        <li><a href="#posts">Search</a></li>
        <li><a href="#subscriptions">Subscibers</a></li>
        <li><a href="#settings">Settings</a></li>
      </ul>
    </div>
  );
}

export default Menu;

