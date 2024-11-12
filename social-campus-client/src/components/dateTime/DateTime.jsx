import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import "./DateTime.css";

function DateTime(props) {
  const [currentTime, setCurrentTime] = useState(new Date());

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentTime(new Date());
    }, 3600000);

    return () => clearInterval(interval);
  }, []);

  const date = new Date(props.dateTime);
  const now = currentTime;
  const timeDifference = now - date;

  if (isNaN(date)) {
    return <span className="creation-time not-general-text">Invalid date</span>;
  }

  const hoursDifference = Math.floor(timeDifference / (1000 * 60 * 60));
  let displayDate = "";

  if (hoursDifference < 24) {
    displayDate = `${hoursDifference} hours ago`;
  } else {
    const currentYear = now.getFullYear();
    const yearOfDate = date.getFullYear();
    if (yearOfDate === currentYear) {
      displayDate = date.toLocaleString(props.locale, {
        month: "short",
        day: "numeric",
      });
    } else {
      displayDate = date.toLocaleString(props.locale, {
        month: "short",
        day: "numeric",
        year: "numeric",
      });
    }
  }

  return <span className="creation-time not-general-text">{displayDate}</span>;
}

DateTime.propTypes = {
  dateTime: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)])
    .isRequired,
  locale: PropTypes.string,
};

export default DateTime;
