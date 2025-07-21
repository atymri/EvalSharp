using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CodeExecution
{
    public class CodeRunner
    {
        public async Task<string> ExecuteCSharpCodeAsync(string code, int timeoutMilliseconds = 5000)
        {
            var options = ScriptOptions.Default
                .AddReferences(
                    typeof(object).Assembly,
                    typeof(Console).Assembly,
                    typeof(Enumerable).Assembly,
                    typeof(List<>).Assembly
                )
                .AddImports("System", "System.Console", "System.Linq", "System.Collections.Generic");

            using var cts = new CancellationTokenSource(timeoutMilliseconds);
            var stringWriter = new StringWriter();
            var originalOut = Console.Out;
            Console.SetOut(stringWriter);

            try
            {
                var result = await CSharpScript.EvaluateAsync(code, options, cancellationToken: cts.Token);
                var consoleOutput = stringWriter.ToString();

                if (!string.IsNullOrWhiteSpace(consoleOutput))
                    return consoleOutput;

                return result?.ToString() ?? "بدون خروجی";
            }
            catch (OperationCanceledException)
            {
                return "خطا: اجرای کد بیش از حد طول کشید و متوقف شد.";
            }
            catch (Exception ex) when (ex.Message.Contains("ReadLine"))
            {
                return "خطا: Console.ReadLine() در این محیط پشتیبانی نمیشود.";
            }
            catch (Exception ex)
            {
                return $"خطا: \n{ex.Message}";
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }
}
