import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import { MANAGER } from "../../../utils/Constants";
import Loading from "../../../utils/Loading";
import { verify } from "../../../utils/Navigation";
import ExtendedGameList from "./parts/ExtendedGameList";

const Manager = () => {
    const navigate = useNavigate();
    const [verified, setVerified] = useState();

    useEffect(() =>{
        verify(MANAGER).then(isVerified => {
            if(!isVerified){
                navigate(AppPaths.home)
                return;
            }
            setVerified(true);
        });
    }, [])

    return (
        <main>
            {
                verified ?
                <div>
                    <ExtendedGameList />
                </div>
                :
                <Loading />
            }
        </main>
    )
}
export default Manager;