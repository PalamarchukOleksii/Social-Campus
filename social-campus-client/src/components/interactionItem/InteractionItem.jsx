import React, { useState } from "react";
import PropTypes from "prop-types";
import "./InteractionItem.css";

function InteractionItem(props) {
  const [isActive, setIsActive] = useState(false);
  const [count, setCount] = useState(props.label);

  const handleClick = () => {
    setIsActive((prev) => !prev);
    setCount((prev) => (isActive ? prev - 1 : prev + 1));

    if (props.onClick) {
      props.onClick();
    }
  };

  return (
    <div className="interaction-item" onClick={handleClick}>
      {isActive ? (
        props.activeIcon ? (
          <props.activeIcon className="active icon" />
        ) : (
          <props.icon className="icon" />
        )
      ) : (
        <props.icon className="icon" />
      )}
      <span className="stat">{count}</span>
    </div>
  );
}

InteractionItem.propTypes = {
  label: PropTypes.number.isRequired,
  icon: PropTypes.elementType.isRequired,
  activeIcon: PropTypes.elementType,
  onClick: PropTypes.func,
};

export default InteractionItem;
