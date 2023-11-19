using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

/*public class TextDocument
{
    public string Content { get; set; }

    public TextDocument(string content)
    {
        Content = content;
    }
}*/

public class Figure
{
    public string Name { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public Figure()
    {
    }
    public override string ToString()
    {
        return $"{Name}\n{Width}\n{Height}\n";
    }

    // Методы для сохранения и загрузки текста в разных 
}

public class TextEditor
{
    private List<Figure> figures;
    private int selectedFigureIndex;

    public TextEditor()
    {
        figures = new List<Figure>();
        selectedFigureIndex = -1;
    }

    public void CreateFigure(string name, double width, double height)
    {
        Figure figure = new Figure { Name = name, Width = width, Height = height };
        figures.Add(figure);
    }

    public void SelectFigure(int index)
    {
        if (index >= 0 && index < figures.Count)
        {
            selectedFigureIndex = index;
        }
        else
        {
            Console.WriteLine("Неверный индекс фигуры.");
        }
    }

    public void EditSelectedFigure()
    {
        if (selectedFigureIndex >= 0 && selectedFigureIndex < figures.Count)
        {
            Console.WriteLine($"Выбрана фигура: {figures[selectedFigureIndex].Name}");
            Console.WriteLine("Введите параметр фигуры, который хотите изменить(Name, Width, Height)");

            switch (Console.ReadLine().ToLower())
            {
                case "name":
                    figures[selectedFigureIndex].Name = Console.ReadLine();
                    break;
                case "width":
                    double value;
                    while (double.TryParse(Console.ReadLine(), out value))
                    {
                        Console.WriteLine("Incorrect Value");
                    }
                    figures[selectedFigureIndex].Width = value;
                    break;
                case "height":
                    double value1;
                    while (double.TryParse(Console.ReadLine(), out value1))
                    {
                        Console.WriteLine("Incorrect Value");
                    }
                    figures[selectedFigureIndex].Height = value1;
                    break;
            }
        }
        else
        {
            Console.WriteLine("Фигура не выбрана.");
        }
    }

    public void PrintFigureDetails()
    {
        if (selectedFigureIndex >= 0 && selectedFigureIndex < figures.Count)
        {
            Figure selectedFigure = figures[selectedFigureIndex];
            Console.WriteLine($"Имя: {selectedFigure.Name}");
            Console.WriteLine($"Ширина: {selectedFigure.Width}");
            Console.WriteLine($"Высота: {selectedFigure.Height}");
        }
        else
        {
            Console.WriteLine("Фигура не выбрана.");
        }
    }

    public void SaveTextToFile(string path)
    {
        foreach (Figure f in figures)
        {
            File.AppendAllText(path, f.ToString());
            figures.Clear();
        }
    }

    public void LoadTextFromFile(string path)
    {
        string[] strings = File.ReadAllLines(path);
        for (int i = 0; i < strings.Length; i += 3)
        {
            CreateFigure(strings[i], Convert.ToDouble(strings[i + 1]), Convert.ToDouble(strings[i + 2]));
        }
    }
    public void SaveTextToJson(string path)
    {
        JsonSerializer.Serialize(File.Open(path, FileMode.OpenOrCreate), figures);
        figures.Clear();
    }
    public void LoadTextFromXml(string fileName)
    {
        if (File.Exists(fileName))
        {
            using (var reader = new StreamReader(fileName))
            {
                var serializer = new XmlSerializer(typeof(List<Figure>));
                figures = (List<Figure>)serializer.Deserialize(reader);
            }
        }
        else
        {
            Console.WriteLine("Файл не найден.");
        }
    }

    public void SaveTextToXml(string fileName)
    {
        using (var writer = new StreamWriter(fileName))
        {
            var serializer = new XmlSerializer(typeof(List<Figure>));
            serializer.Serialize(writer, figures);
        }
        figures.Clear();
    }

    public void LoadTextFromJson(string fileName)
    {
        if (File.Exists(fileName))
        {
            string json = File.ReadAllText(fileName);
            figures = JsonSerializer.Deserialize<List<Figure>>(json);
        }
        else
        {
            Console.WriteLine("Файл не найден.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Текстовый редактор");

        TextEditor editor = new TextEditor();

        while (true)
        {
            Console.WriteLine("Введите команду (create, select, edit, save-txt, load-txt, save-json, load-json, save-xml, load-xml, print, exit):");
            string command = Console.ReadLine();

            if (command == "create")
            {
                Console.WriteLine("Введите имя, ширину и высоту фигуры:");
                string name = Console.ReadLine();
                double width = double.Parse(Console.ReadLine());
                double height = double.Parse(Console.ReadLine());
                editor.CreateFigure(name, width, height);
            }
            else if (command == "select")
            {
                Console.WriteLine("Введите индекс фигуры:");
                if (int.TryParse(Console.ReadLine(), out int index))
                {
                    editor.SelectFigure(index);
                }
                else
                {
                    Console.WriteLine("Неверный формат индекса.");
                }
            }
            else if (command == "edit")
            {
                editor.EditSelectedFigure();
            }
            else if (command == "save-txt")
            {
                Console.WriteLine("Введите имя файла для сохранения текста (txt):");
                string fileName = Console.ReadLine();
                editor.SaveTextToFile(fileName);
                Console.WriteLine($"Текст сохранен в файл: {fileName}");
            }
            else if (command == "load-txt")
            {
                Console.WriteLine("Введите имя файла для загрузки текста (txt):");
                string fileName = Console.ReadLine();
                editor.LoadTextFromFile(fileName);
                Console.WriteLine($"Текст загружен из файла: {fileName}");
            }
            else if (command == "save-json")
            {
                Console.WriteLine("Введите имя файла для сохранения текста (json):");
                string fileName = Console.ReadLine();
                editor.SaveTextToJson(fileName);
                Console.WriteLine($"Текст сохранен в JSON файл: {fileName}");
            }
            else if (command == "load-json")
            {
                Console.WriteLine("Введите имя файла для загрузки текста (json):");
                string fileName = Console.ReadLine();
                editor.LoadTextFromJson(fileName);
                Console.WriteLine($"Текст загружен из JSON файла: {fileName}");
            }
            else if (command == "save-xml")
            {
                Console.WriteLine("Введите имя файла для сохранения текста (xml):");
                string fileName = Console.ReadLine();
                editor.SaveTextToXml(fileName);
                Console.WriteLine($"Текст сохранен в XML файл: {fileName}");
            }
        }
    }
}