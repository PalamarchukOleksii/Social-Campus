import React, { useState } from "react";
import NavItem from "../../components/navItem/NavItem";
import FollowTabsItems from "../../utils/consts/FollowTabsItems";
import "./FollowTabs.css";

function FollowTabs() {
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
            path={path}
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
