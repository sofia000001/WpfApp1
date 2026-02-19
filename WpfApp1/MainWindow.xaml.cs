using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private string currentFilePath = null;
        private bool isTextChanged = false;

        public MainWindow()
        {
            InitializeComponent();
            OutputTextBox.Text = "Результаты работы будут отображаться здесь.\n";
        }

        // ФАЙЛ 
        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (CheckSaveChanges())
            {
                EditorTextBox.Clear();
                currentFilePath = null;
                isTextChanged = false;
                UpdateTitle();
                StatusText.Text = "Создан новый файл";
                OutputTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Новый файл создан\n");
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (CheckSaveChanges())
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

                if (dlg.ShowDialog() == true)
                {
                    try
                    {
                        EditorTextBox.Text = File.ReadAllText(dlg.FileName);
                        currentFilePath = dlg.FileName;
                        isTextChanged = false;
                        UpdateTitle();
                        StatusText.Text = $"Открыт: {dlg.FileName}";
                        OutputTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Открыт файл: {dlg.FileName}\n");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveAs_Click(sender, e);
            }
            else
            {
                try
                {
                    File.WriteAllText(currentFilePath, EditorTextBox.Text);
                    isTextChanged = false;
                    UpdateTitle();
                    StatusText.Text = $"Сохранено: {currentFilePath}";
                    OutputTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Файл сохранен\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dlg.FileName, EditorTextBox.Text);
                    currentFilePath = dlg.FileName;
                    isTextChanged = false;
                    UpdateTitle();
                    StatusText.Text = $"Сохранено как: {dlg.FileName}";
                    OutputTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Файл сохранен как: {dlg.FileName}\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (CheckSaveChanges())
            {
                Application.Current.Shutdown();
            }
        }

        // ПРАВКА

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (EditorTextBox.CanUndo)
            {
                EditorTextBox.Undo();
                StatusText.Text = "Отмена действия";
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (EditorTextBox.CanRedo)
            {
                EditorTextBox.Redo();
                StatusText.Text = "Возврат действия";
            }
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            EditorTextBox.Cut();
            StatusText.Text = "Вырезано";
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            EditorTextBox.Copy();
            StatusText.Text = "Скопировано";
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            EditorTextBox.Paste();
            StatusText.Text = "Вставлено";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            EditorTextBox.SelectedText = "";
            StatusText.Text = "Удалено";
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            EditorTextBox.SelectAll();
            StatusText.Text = "Выделено всё";
        }

        // СПРАВКА 

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            string helpText = "СПРАВКА - реализованные функции:\n\n" +
                            "МЕНЮ ФАЙЛ:\n" +
                            "- Создать: создает новый файл\n" +
                            "- Открыть: открывает существующий файл\n" +
                            "- Сохранить: сохраняет текущий файл\n" +
                            "- Сохранить как: сохраняет файл с новым именем\n" +
                            "- Выход: выход из программы\n\n" +
                            "МЕНЮ ПРАВКА:\n" +
                            "- Отменить: отменяет последнее действие\n" +
                            "- Вернуть: возвращает отмененное действие\n" +
                            "- Вырезать: вырезает выделенный текст\n" +
                            "- Копировать: копирует выделенный текст\n" +
                            "- Вставить: вставляет текст из буфера\n" +
                            "- Удалить: удаляет выделенный текст\n" +
                            "- Выделить всё: выделяет весь текст\n\n" +
                            "МЕНЮ СПРАВКА:\n" +
                            "- Справка: отображает данное окно\n" +
                            "- О программе: информация о приложении";

            MessageBox.Show(helpText, "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            string aboutText = "Разработка пользовательского интерфейса (GUI) для языкового процессора\n" +
                             "Лабораторная работа №1\n\n" +
                             "Автор: Дарчук Софья\n\n" +
                             "Программа является основой для языкового процессора\n" +
                             "анализа исходного кода.";

            MessageBox.Show(aboutText, "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
            OutputTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Открыто окно 'О программе'\n");
        }

        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ 

        private void EditorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isTextChanged = true;
            UpdateTitle();
        }

        private bool CheckSaveChanges()
        {
            if (isTextChanged)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Сохранить изменения в файле?",
                    "Сохранение",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(null, null);
                    return true;
                }
                else if (result == MessageBoxResult.No)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private void UpdateTitle()
        {
            string title = "Текстовый редактор";
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                title += $" - {System.IO.Path.GetFileName(currentFilePath)}";
            }
            if (isTextChanged)
            {
                title += "*";
            }
            this.Title = title;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!CheckSaveChanges())
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }
    }
}