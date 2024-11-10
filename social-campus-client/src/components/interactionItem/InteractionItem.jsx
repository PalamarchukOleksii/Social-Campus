import React, { useState } from "react";
import PropTypes from "prop-types";
import "./InteractionItem.css";
import { useNavigate } from "react-router-dom";

function InteractionItem(props) {
  const [isActive, setIsActive] = useState(false);
  const [count, setCount] = useState(props.label);
  const navigate = useNavigate();

  const handleClick = () => {
    if (props.itemType === "comment") {
      navigate(`/publication/${props.publicationId}`);
    } else {
      setIsActive((prev) => !prev);
      setCount((prev) => (isActive ? prev - 1 : prev + 1));
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
  itemType: PropTypes.string.isRequired,
  label: PropTypes.number.isRequired,
  icon: PropTypes.elementType.isRequired,
  activeIcon: PropTypes.elementType,
  publicationId: PropTypes.number.isRequired,
};

export default InteractionItem;
