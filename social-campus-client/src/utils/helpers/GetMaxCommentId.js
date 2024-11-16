import userData from "../../data/userData.json";

const getMaxCommentId = () => {
  let maxId = 0;
  userData.forEach((user) => {
    user.publications.forEach((publication) => {
      publication.comments.forEach((comment) => {
        if (comment.id > maxId) {
          maxId = comment.id;
        }
      });
    });
  });

  return maxId;
};

export default getMaxCommentId;
