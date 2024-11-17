import React, { useState } from "react";
import { useParams } from "react-router-dom";
import NavItem from "../../components/navItem/NavItem";
import FollowTabsItems from "../../utils/consts/FollowTabsItems";
import "./FollowTabs.css";

function FollowTabs() {
  const { login } = useParams();
  const [hoveredIcon, setHoveredIcon] = useState("");

  return (
    <div className="tabs">
      {FollowTabsItems.map(
        ({
          path,
          label,
          inactiveIcon: InactiveIcon,
          activeIcon: ActiveIcon,
        }) => (
          <NavItem
            key={path}
            path={
              path.includes(":login") ? path.replace(":login", login) : path
            }
            label={label}
            inactiveIcon={InactiveIcon}
            activeIcon={ActiveIcon}
            hoveredIcon={hoveredIcon}
            setHoveredIcon={setHoveredIcon}
          />
        )
      )}
    </div>
  );
}

export default FollowTabs;
