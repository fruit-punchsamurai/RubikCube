import cv2
import numpy as np
import kociemba
import socket

host, port = "127.0.0.1", 25001


sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host, port))

color_dect = {
    'R': (0, 0, 255),
    'L': (0, 165, 255),
    'B': (255, 0, 0),
    'F': (0, 255, 0),
    'U': (255, 255, 255),
    'D': (0, 255, 255)
}

def draw_grid(frame, grid_size, square_size, gap,hsv_frame):
    height, width = frame.shape[:2]
    start_x, start_y = (width - (square_size * grid_size + gap * (grid_size - 1))) // 2, (height - (square_size * grid_size + gap * (grid_size - 1))) // 2
    squares = []
    for i in range(grid_size):
        row = []
        for j in range(grid_size):
            x, y = start_x + j * (square_size + gap), start_y + i * (square_size + gap)
            row.append((x, y, x + square_size, y + square_size))
        squares.append(row)

    colors = detect_colors(hsv_frame, squares)
    for i in range(grid_size):
        for j in range(grid_size):
            x, y = start_x + j * (square_size + gap), start_y + i * (square_size + gap)
            square_roi = hsv_frame[y:y+square_size, x:x+square_size]
            avg_color = np.mean(square_roi, axis=(0, 1))
            color = (get_color_name(avg_color))
            if color == 'unknown':
              rect_color = (0,0,0)
            else:
                rect_color = color_dect[color]
            cv2.rectangle(frame, (x, y), (x + square_size, y + square_size),rect_color , 2)
    return colors



def detect_colors(hsv_frame, squares):
    color_names = []
    for row in squares:
        color_row = []
        for (x1, y1, x2, y2) in row:
            square_roi = hsv_frame[y1:y2, x1:x2]
            avg_color = np.mean(square_roi, axis=(0, 1))
            color_row.append(get_color_name(avg_color))
        color_names.append(color_row)
    return color_names

def get_color_name(hsv_val):
    h, s, v = hsv_val
    red_lower1 = (0, 50, 50)
    red_upper1 = (2.99, 255, 255)
    red_lower2 = (150, 50, 50)
    red_upper2 = (180, 255, 255)

    orange_lower = (3, 50, 50)
    orange_upper = (20, 255, 255)

    yellow_lower = (21, 50, 50)
    yellow_upper = (40, 255, 255)

    green_lower = (40, 50, 50)
    green_upper = (85, 255, 255)

    white_lower = (0, 0, 80)
    white_upper = (180, 50, 255)

    blue_lower = (86, 50, 50)
    blue_upper = (130, 255, 255)

    if check_in_range(hsv_val, white_lower, white_upper):
        return "U"
    elif check_in_range(hsv_val, red_lower2, red_upper2) or check_in_range(hsv_val, red_lower1, red_upper1):
        return "R"
    elif check_in_range(hsv_val, orange_lower, orange_upper):
        return "L"
    elif check_in_range(hsv_val, yellow_lower, yellow_upper):
        return "D"
    elif check_in_range(hsv_val, green_lower, green_upper):
        return "F"
    elif check_in_range(hsv_val, blue_lower, blue_upper):
        return "B"
    else:
        return "unknown"

def check_in_range(val, lower, upper):
    return lower[0] <= val[0] <= upper[0] and lower[1] <= val[1] <= upper[1] and lower[2] <= val[2] <= upper[2]

def SendDataToUnity(data):
    # SOCK_STREAM means TCP socket
   
    # Connect to the server and send the data
    print("SendingData")
    sock.sendall(data.encode("utf-8"))
    print("DataSent")
    #response = sock.recv(1024).decode("utf-8")
    #print (response)

captureFlag = False;
cubee = ""
cap = cv2.VideoCapture(0)
cube = ["","","","","",""]
i = 0;
while True:

    
    if i==6:
        string = cube[4]+cube[1]+cube[0]+cube[5]+cube[3]+cube[2]
        initialState = "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB"
        #testString = "FBURUFDRULURFRLLBRFFBRFDRDDBLFFDDBUULBRLLDDLDBUUBBUFRL";
        scrambleSteps = "z"+kociemba.solve(initialState,string)
        print(scrambleSteps)
        SendDataToUnity(scrambleSteps)
        responseScramble = sock.recv(1024).decode("utf-8")
        print(responseScramble)
        ans = "x"+kociemba.solve(string)
        print(ans)
        SendDataToUnity(ans)
        responseSolution = sock.recv(1024).decode("utf-8")
        print(responseSolution)
        #print(ans)
        response1 = sock.recv(1024).decode("utf-8");
        if response1 == "close":
            break
        
    ret, frame = cap.read()
    if not ret:
        break

    hsv_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
    # squares = draw_grid(frame, 3, 80, 150) 
    colors = draw_grid(frame,3,50,90,hsv_frame)
    # colors = detect_colors(hsv_frame, squares)
    
    if i==0:
        cv2.putText(frame, "green", (50,35), cv2.FONT_HERSHEY_SIMPLEX, 1, (255,0,0), 2)
    elif i==1:
        cv2.putText(frame, "red", (50,35), cv2.FONT_HERSHEY_SIMPLEX, 1, (255,0,0), 2)
    elif i==2:
        cv2.putText(frame, "blue", (50,35), cv2.FONT_HERSHEY_SIMPLEX, 1, (255,0,0), 2)
    elif i==3:
        cv2.putText(frame, "orange", (50,35), cv2.FONT_HERSHEY_SIMPLEX, 1, (255,0,0), 2)
    elif i==4:
        cv2.putText(frame, "white", (50,35), cv2.FONT_HERSHEY_SIMPLEX, 1, (255,0,0), 2)
    else:
        cv2.putText(frame, "yellow", (50,35), cv2.FONT_HERSHEY_SIMPLEX, 1, (255,0,0), 2) 

    cv2.putText(frame, cubee, (400,35), cv2.FONT_HERSHEY_SIMPLEX, 1, (255,0,0), 2)
    if cv2.waitKey(1) & 0xFF == ord('c'):
        # print("hello")
        captureFlag = True
        cubee=""
        for color_row in colors:
            for color in color_row:
              # print(i)
              cubee+=color
              cube[i]+=color
            # print("Colors:", color_row)
        
        print(cube)
        i+=1
    cv2.imshow("Rubik's Cube Detector", frame)
    
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

sock.close()
cap.release()
cv2.destroyAllWindows()


