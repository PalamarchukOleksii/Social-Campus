import userData from "../../data/userData.json";

const getMaxPublicationId = () => {
  let maxId = 0;
  userData.forEach((user) => {
    user.publications.forEach((pub) => {
      if (pub.id > maxId) {
        maxId = pub.id;
      }
    });
  });
  return maxId;
};

export default getMaxPublicationId;
