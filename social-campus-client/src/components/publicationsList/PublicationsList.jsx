import React from "react";
import PropTypes from "prop-types";
import Publication from "../publication/Publication";
import "./PublicationsList.css";

function PublicationsList(props) {
  return (
    <div className="publications">
      {props.publications.map((publication) => (
        <Publication
          key={publication.id}
          publicationId={publication.id}
          description={publication.description}
          imageUrl={publication.imageUrl}
          creationTime={publication.creationTime}
          likesCount={publication.likesCount}
          commentsCount={publication.comments.length}
          username={publication.username}
          login={publication.login}
          profileImage={publication.profileImage}
        />
      ))}
    </div>
  );
}

PublicationsList.propTypes = {
  publications: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
      description: PropTypes.string,
      imageUrl: PropTypes.string,
      creationTime: PropTypes.string,
      likesCount: PropTypes.number,
      comments: PropTypes.arrayOf(PropTypes.object).isRequired,
      username: PropTypes.string,
      login: PropTypes.string,
      profileImage: PropTypes.string,
    })
  ).isRequired,
};

export default PublicationsList;
