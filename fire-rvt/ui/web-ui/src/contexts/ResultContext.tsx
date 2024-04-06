import React, {
  createContext,
  useState,
  Dispatch,
  SetStateAction,
  ReactNode,
} from "react";

// TODO: Props to be changed based on result output of AI model
interface CheckResultProps {
  id: string;
  room_name: string;
  room_area: number;
  room_vertices: [number, number][];
  extinguisher_vertices: [number, number][];
  path_vertices: [number, number][];
  rating: number;
  result: string;
}

// TODO: Props to be changed based on result output of AI model
interface InferResultProps {
  id: string;
  room_name: string;
  room_area: number;
  room_vertices: [number, number][];
  extinguisher_vertices: [number, number][];
  path_vertices: [number, number][];
  rating: number;
  result: string;
}

interface RoomProps {
  id : string;
  name : string;
  level : string;
  vertices: number[][][][];
  //extinguisher_vertices: [number, number][] | null;
}

interface FloorProps {
  id: string;
  name: string;
  rooms : RoomProps[] | null; // TODO: change to RoomProps
}

interface ResultContextType {
  checkResultData: CheckResultProps[];
  setCheckResultData: Dispatch<SetStateAction<CheckResultProps[]>>;
  inferResultData: InferResultProps[];
  setInferResultData: Dispatch<SetStateAction<InferResultProps[]>>;
  
  allRooms : RoomProps[];
  setAllRooms : Dispatch<SetStateAction<RoomProps[]>>;
  allFloors : FloorProps[];
  setAllFloors : Dispatch<SetStateAction<FloorProps[]>>;

  currentRoom : RoomProps;
  setCurrentRoom : Dispatch<SetStateAction<RoomProps>>;
  currentFloor : FloorProps;
  setCurrentFloor : Dispatch<SetStateAction<FloorProps>>;
}

interface Props {
  children: ReactNode;
}

export const ResultContext = createContext<ResultContextType>({
  checkResultData: [],
  setCheckResultData: () => {},
  inferResultData: [],
  setInferResultData: () => {},
  
  allFloors: [],
  setAllFloors : () => {},
  allRooms : [],
  setAllRooms : () => {},

  currentRoom : {"id":"", "name":"", "level":"", "vertices":[[[[0,0]]]]},
  setCurrentRoom : () => {},
  currentFloor: {"id":"", "name":"", "rooms":[]},
  setCurrentFloor : () => {}

});

export const ResultContextProvider: React.FC<Props> = ({ children }) => {
  const [checkResultData, setCheckResultData] = useState<CheckResultProps[]>(
    [],
  );
  const [inferResultData, setInferResultData] = useState<InferResultProps[]>(
    [],
  );
  const [allFloors, setAllFloors] = useState<FloorProps[]>([]);
  const [allRooms, setAllRooms] = useState<RoomProps[]>([]);

  const [currentRoom, setCurrentRoom] = useState<RoomProps>(
    {"id":"", "name":"", "level":"", "vertices":[[[[0,0]]]]}
  );
  const [currentFloor, setCurrentFloor] = useState<FloorProps>(
    {"id":"", "name":"", "rooms":[]}
  );
  return (
    <ResultContext.Provider
      value={{
        checkResultData,setCheckResultData,
        inferResultData,setInferResultData,
        allFloors, setAllFloors,
        allRooms, setAllRooms,
        currentRoom, setCurrentRoom,
        currentFloor, setCurrentFloor
      }}
    >
      {children}
    </ResultContext.Provider>
  );
};
