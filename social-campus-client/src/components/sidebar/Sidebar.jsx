import React, { useEffect, useState } from "react";
import "./Sidebar.css";
import { useNavigate } from "react-router-dom";
import { IoExit, IoExitOutline } from "react-icons/io5";
import NavItem from "../navItem/NavItem";
import ShortProfile from "../shortProfile/ShortProfile";
import SidebarItems from "../../utils/consts/SidebarItems";
import userData from "../../data/userData.json";
import login from "../../utils/consts/AuthUserLogin";

function Sidebar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState("");
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserData = () => {
      const foundUser = userData.find((user) => user.login === login);
      setUser(foundUser || null);
      setLoading(false);
    };

    fetchUserData();
  }, []);

  if (loading) {
    return <div className="sidebar-loading">Loading...</div>;
  }

  if (!user) {
    return <p>User not found.</p>;
  }

  const handleLogout = async (e) => {
    e.preventDefault();
    try {
      // Logout logic
      console.log("User logged out");
      navigate("/");
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="sidebar">
      <div className="wrapper">
        <div className="head">
          <img src="/android-chrome-512x512.png" alt="logo" />
          <span>Social Campus</span>
        </div>
        <div className="navigation">
          <ul>
            {SidebarItems.map(
              ({
                path,
                label,
                inactiveIcon: InactiveIcon,
                activeIcon: ActiveIcon,
              }) => {
                return (
                  <li key={path}>
                    <NavItem
                      path={path}
                      label={label}
                      inactiveIcon={InactiveIcon}
                      activeIcon={ActiveIcon}
                      hoveredIcon={hoveredIcon}
                      setHoveredIcon={setHoveredIcon}
                    />
                  </li>
                );
              }
            )}
          </ul>
        </div>
      </div>
      <div className="logout">
        <ShortProfile
          username={user.username}
          login={user.login}
          profileImage={user.profileImage}
        />
        <div
          onClick={handleLogout}
          className="logout-icon"
          onMouseEnter={() => setHoveredIcon("logout")}
          onMouseLeave={() => setHoveredIcon("")}
        >
          {hoveredIcon === "logout" ? (
            <IoExit className="exit-icon" />
          ) : (
            <IoExitOutline className="exit-icon" />
          )}
        </div>
      </div>
    </div>
  );
}

export default Sidebar;
