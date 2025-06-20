import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Publication from "../../components/publication/Publication";
import NavItem from "../../components/navItem/NavItem";
import PublicationDetailItems from "../../utils/consts/PublicationDetailItems";
import "./PublicationDetail.css";
import CreateComment from "../../components/createComment/CreateComment";
import CommentReplyManager from "../../components/comment/CommentReplyManager";
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

const GET_PUBLICATION_URL = "/api/publications/";
const GET_COMMENTS_URL = "/api/publications/";
const PAGE_SIZE = 10;

function PublicationDetail() {
  const { id } = useParams();
  const { auth } = useAuth();
  const axios = useAxiosPrivate();
  const navigate = useNavigate();

  const [publication, setPublication] = useState(null);
  const [loading, setLoading] = useState(true);
  const [hoveredIcon, setHoveredIcon] = useState("");
  const [comments, setComments] = useState([]);
  const [commentPage, setCommentPage] = useState(1);
  const [commentsLoading, setCommentsLoading] = useState(false);
  const [allCommentsFetched, setAllCommentsFetched] = useState(false);

  useEffect(() => {
    const fetchPublication = async () => {
      try {
        setLoading(true);
        const pubResponse = await axios.get(`${GET_PUBLICATION_URL}${id}`);
        setPublication(pubResponse.data);
      } catch (error) {
        console.error("Failed to fetch publication:", error);
        setPublication(null);
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchPublication();
    }
  }, [id, axios]);

  const fetchComments = async () => {
    if (allCommentsFetched || !id) return;

    try {
      setCommentsLoading(true);
      const response = await axios.get(
        `${GET_COMMENTS_URL}${id}/comments/count/${PAGE_SIZE}/page/${commentPage}`
      );

      const newComments = response.data;

      if (newComments.length < PAGE_SIZE) {
        setAllCommentsFetched(true);
      }

      if (newComments.length === 0) return;

      setComments((prev) => [...prev, ...newComments]);
      setCommentPage((prev) => prev + 1);
    } catch (error) {
      console.error("Failed to fetch comments:", error);
    } finally {
      setCommentsLoading(false);
    }
  };

  useEffect(() => {
    fetchComments();
  }, [id]);

  return (
    <div className="publication-detail-container">
      <div className="top-section">
        <NavItem
          label={PublicationDetailItems.label}
          inactiveIcon={PublicationDetailItems.inactiveIcon}
          activeIcon={PublicationDetailItems.activeIcon}
          hoveredIcon={hoveredIcon}
          setHoveredIcon={setHoveredIcon}
          onClick={() => navigate(-1)}
        />
        <h1 className="general-text">Publication</h1>
      </div>
      {loading ? (
        <></>
      ) : !publication ? (
        <h1 className="not-found-text general-text">Publication not found</h1>
      ) : (
        <>
          <Publication
            publicationId={publication.id.value}
            disableCreateComment={true}
          />
          <CreateComment publicationId={publication.id.value} />
          {comments.length > 0 ? (
            <>
              <h2 className="comment-section-text general-text">Comments</h2>
              <div className="comments-section">
                {comments.map((comment) => (
                  <CommentReplyManager
                    key={comment.id.value}
                    comment={comment}
                    currentUser={auth.shortUser}
                    comments={comments}
                    setComments={setComments}
                  />
                ))}
              </div>
              {!allCommentsFetched && (
                <div className="load-more-container">
                  <button onClick={fetchComments} disabled={commentsLoading}>
                    Load More
                  </button>
                </div>
              )}
            </>
          ) : (
            <h2 className="general-text no-comments">No comments available</h2>
          )}
        </>
      )}
    </div>
  );
}

export default PublicationDetail;
