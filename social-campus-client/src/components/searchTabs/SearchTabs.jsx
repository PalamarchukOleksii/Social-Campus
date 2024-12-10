import React, { useState } from "react";
import NavItem from "../../components/navItem/NavItem";
import SearchTabsItems from "../../utils/consts/SearchTabsItems";
import "./SearchTabs.css";

function SearchTabs() {
  const [hoveredIcon, setHoveredIcon] = useState("");

  return (
    <div className="tabs">
      {SearchTabsItems.map(
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

export default SearchTabs;
