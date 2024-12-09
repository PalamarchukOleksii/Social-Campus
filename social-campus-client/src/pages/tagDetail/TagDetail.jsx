import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import jsonData from "../../data/userData.json";
import "./TagDetail.css";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import NavItem from "../../components/navItem/NavItem";
import PublicationDetailItems from "../../utils/consts/PublicationDetailItems";

function TagDetail() {
  const { tag } = useParams();
  const navigate = useNavigate();
  const [relatedPublications, setRelatedPublications] = useState([]);
  const [hoveredIcon, setHoveredIcon] = useState("");

  useEffect(() => {
    const filteredPublications = [];

    jsonData.forEach((user) => {
      user.publications.forEach((publication) => {
        const regex = new RegExp(`#${tag}\\b`, "g");
        const matches = publication.description.match(regex);

        if (matches) {
          filteredPublications.push({
            ...publication,
            username: user.username,
            login: user.login,
            profileImage: user.profileImage,
            userId: user.id,
          });
        }
      });
    });

    setRelatedPublications(filteredPublications);
  }, [tag]);

  return (
    <div className="tag-detail-container">
      <div className="top-section">
        <NavItem
          label={PublicationDetailItems.label}
          inactiveIcon={PublicationDetailItems.inactiveIcon}
          activeIcon={PublicationDetailItems.activeIcon}
          hoveredIcon={hoveredIcon}
          setHoveredIcon={setHoveredIcon}
          onClick={() => navigate(-1)}
        />
        <h1 className="general-text">Publications for #{tag}</h1>
      </div>
      {relatedPublications.length > 0 ? (
        <PublicationsList publications={relatedPublications} />
      ) : (
        <h2 className="not-found-users general-text">
          No publications found for this tag.
        </h2>
      )}
    </div>
  );
}

export default TagDetail;
