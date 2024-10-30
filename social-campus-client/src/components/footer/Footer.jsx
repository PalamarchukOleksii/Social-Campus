import React from "react";
import "./Footer.css";

function Footer() {
  return (
    <footer>
      <div className="footer">
        <p className="not-general-text">
          &copy; {new Date().getFullYear()} Social Campus. All rights reserved.
        </p>
      </div>
    </footer>
  );
}

export default Footer;
