using System.IO;

public class Flock
{
    public Drone[] agents;
    public int num;
    
    public Flock(int maxnum)
    {
        agents = new Drone[maxnum];
    }
    
    // actually add the drones
    public void Init(int num)
    {
        this.num = num;
        for (int i=0; i<num; i++)
        {
            agents[i] = new Drone(i);
        }
    }
    
    public void Update()
    {
        for (int i=0; i<num; i++)
        {
            agents[i].Update();
        }
    }
    
    public float average() 
    {
        float totalTemp = 0;
        for (int i = 0; i < num; i++)
        {
            totalTemp += agents[i].Temperature;
        }
        Console.WriteLine("Average Temp: " + totalTemp/num);
        return 0;
    }

    public int max()
{
    if (num == 0)
        return -1;

    float maxTemp = agents[0].Temperature; // Start with the first drone's temperature.
    int maxID = agents[0].ID; // Track the ID of the drone with the highest temperature.
    
    for (int i = 1; i < num; i++)
    {
        if (agents[i].Temperature > maxTemp)
        {
            maxTemp = agents[i].Temperature;
            maxID = agents[i].ID;
        }
    }

    Console.WriteLine("Max Temp Value: " + maxID.ToString());
    
    return maxID; // Return the ID of the drone with the maximum temperature.
}

    public int min()
        {
            if (num == 0) return 0;

            int minValue = agents[0].ID;
            for (int i = 1; i < num; i++)
            {
                if (agents[i].ID < minValue)
                {
                    minValue = agents[i].ID;
                }
            }
            return minValue;
        }

     public void print(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write header
            writer.WriteLine("ID,Temperature,Wind,Battery");

            // Write each drone's data
            for (int i = 0; i < num; i++)
            {
                Drone drone = agents[i];
                writer.WriteLine($"{drone.ID},{drone.Temperature},{drone.Wind},{drone.Battery}");
            }
        }
    }

    public void append(Drone val)
    {
    }

    public void appendFront(Drone val)
    {
    }


    public void insert(Drone val, int index)
    {

    }

    public void deleteFront(int index)
    {

    }

    public void deleteBack(int index)
    {

    }


    public void delete(int index)
    {

    } 
    
    
    public void bubblesort(string attribute)
{
    
    for (int i = 0; i < num - 1; i++)
    {
        for (int j = 0; j < num - i - 1; j++)
        {
            // Compare based on the selected attribute
            bool swap = false;
            switch (attribute.ToLower())
            {
                case "temp":
                    if (agents[j].Temperature > agents[j + 1].Temperature)
                    {
                        swap = true;
                    }
                    break;
                case "wind":
                    if (agents[j].Wind > agents[j + 1].Wind)
                    {
                        swap = true;
                    }
                    break;
                case "battery":
                    if (agents[j].Battery > agents[j + 1].Battery)
                    {
                        swap = true;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid attribute for sorting. Please use 'temp', 'wind', or 'battery'.");
                    return; // Exit the method if the attribute is invalid
            }

            if (swap)
            {
                // Swap the drones
                Drone temp = agents[j];
                agents[j] = agents[j + 1];
                agents[j + 1] = temp;
            }
        }
    }
}


    public void insertionsort()
    {
        
    }
}