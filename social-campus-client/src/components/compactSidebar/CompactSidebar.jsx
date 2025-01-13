import React, { useEffect, useState } from "react";
import "./CompactSidebar.css";
import { useNavigate } from "react-router-dom";
import { IoExit, IoExitOutline } from "react-icons/io5";
import NavItem from "../navItem/NavItem";
import userData from "../../data/userData.json";
import login from "../../utils/consts/AuthUserLogin";
import { useCreateItem } from "../../context/CreateItemContext";
import { IoAdd } from "react-icons/io5";
import useAuth from "../../hooks/useAuth";
import GetSidebarItems from "../../utils/consts/GetSidebarItems";

function CompactSidebar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState("");
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  const { openCreatePublication } = useCreateItem();
  const { auth } = useAuth();

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

  const sidebarItems = GetSidebarItems(auth.shortUser.login);

  return (
    <div className="compact-sidebar">
      <div className="wrapper">
        <div className="head">
          <img src="/android-chrome-512x512.png" alt="logo" />
        </div>
        <div className="navigation">
          <ul>
            {sidebarItems.map(
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
                    showLabel={false}
                  />
                </li>
              )
            )}
          </ul>
          <div className="button-wrapper">
            <button className="add-publish-short" onClick={handlePublishClick}>
              <IoAdd />
            </button>
          </div>
        </div>
      </div>
      {loading ? (
        <></>
      ) : user ? (
        <div className="compact-logout">
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

export default CompactSidebar;
