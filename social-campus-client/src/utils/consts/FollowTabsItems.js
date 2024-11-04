import { IoArrowBackCircleOutline, IoArrowBackCircle } from "react-icons/io5"; // Import your icon components
import ROUTES from "./Routes";

const FollowTabsItems = [
  {
    path: `${ROUTES.PROFILE}`,
    label: "Profile",
    inactiveIcon: IoArrowBackCircleOutline,
    activeIcon: IoArrowBackCircle,
  },
  {
    path: `${ROUTES.FOLLOWING}`,
    label: "Following",
  },
  {
    path: `${ROUTES.FOLLOWERS}`,
    label: "Followers",
  },
];

export default FollowTabsItems;
