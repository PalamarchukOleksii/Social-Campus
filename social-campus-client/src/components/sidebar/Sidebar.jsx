import React, { useEffect, useState } from "react";
import "./Sidebar.css";
import { useNavigate } from "react-router-dom";
import { IoExit, IoExitOutline } from "react-icons/io5";
import NavItem from "../navItem/NavItem";
import SidebarItems from "../../utils/consts/SidebarItems";
import ShortProfile from "../shortProfile/ShortProfile";
import userData from "../../data/userData.json";
import login from "../../utils/consts/AuthUserLogin";
import { useCreateItem } from "../../context/CreateItemContext";

function Sidebar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState("");
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  const { openCreatePublication } = useCreateItem();

  useEffect(() => {
    const fetchUserData = () => {
      const foundUser = userData.find((user) => user.login === login);
      setUser(foundUser || null);
      setLoading(false);
    };

    fetchUserData();
  }, []);

  const handleLogout = async (e) => {
    e.preventDefault();
    try {
      navigate("/");
    } catch (error) {
      console.error(error);
    }
  };

  const handlePublishClick = () => {
    openCreatePublication();
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
              }) => (
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
              )
            )}
          </ul>
          <div className="button-wrapper">
            <button className="add-publish" onClick={handlePublishClick}>
              Publish
            </button>
          </div>
        </div>
      </div>
      {loading ? (
        <></>
      ) : user ? (
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
      ) : (
        <h3 className="not-found-text general-text">User not found</h3>
      )}
    </div>
  );
}

export default Sidebar;
