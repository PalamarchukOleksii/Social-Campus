import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import Publication from "../publication/Publication";
import Comment from "../comment/Comment";
import "./PublicationDetail.css";
import publicationDetailsData from "../../data/publicationDetailsData.json";

function PublicationDetail({ publication }) {
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchComments = async () => {
      try {
        // fetch real data
        const data = publicationDetailsData;
        setComments(data);
      } catch (error) {
        console.error("Error fetching comments:", error);
      } finally {
        setLoading(false);
      }
    };

    if (publication?.id) {
      fetchComments();
    }
  }, [publication]);

  return (
    <div className="publication-detail-container">
      <Publication {...publication} />

      <h3>Comments:</h3>
      <div className="comments-section">
        {loading ? (
          <p>Loading comments...</p>
        ) : comments.length > 0 ? (
          comments.map((comment, index) => (
            <Comment
              key={index}
              username={comment.username}
              login={comment.login}
              profileImage={comment.profileImage}
              text={comment.text}
              likeCount={comment.likeCount}
              creationTime={comment.creationTime}
            />
          ))
        ) : (
          <p>No comments available</p>
        )}
      </div>
    </div>
  );
}

PublicationDetail.propTypes = {
  publication: PropTypes.shape({
    id: PropTypes.number.isRequired,
    description: PropTypes.string.isRequired,
    imageUrl: PropTypes.string,
    creationTime: PropTypes.string.isRequired,
    username: PropTypes.string.isRequired,
    login: PropTypes.string.isRequired,
    profileImage: PropTypes.string,
    likesCount: PropTypes.number,
    commentsCount: PropTypes.number,
  }).isRequired,
};

export default PublicationDetail;
