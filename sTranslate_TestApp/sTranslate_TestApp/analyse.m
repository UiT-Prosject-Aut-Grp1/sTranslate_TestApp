close all;
clear all;

% Read files and add to array
logging{1} = csvread('logCSharpNoCache.csv');
logging{2} = csvread('logCSharpWithCache.csv');
logging{3} = csvread('logFSharpDirectNoCache.csv');
logging{4} = csvread('logFSharpDirectWithCache.csv');
logging{5} = csvread('logFSharpNoCache.csv');
logging{6} = csvread('logFSharpWithCache.csv');
logging{7} = csvread('logFSharpParallel.csv');

% Set names
name{1} = 'C# no cache';
name{2} = 'C# width cache';
name{3} = 'F# Direct no cache';
name{4} = 'F# Direct width cache';
name{5} = 'F# No Cache';
name{6} = 'F# with cache';
name{7} = 'F# Parallel';


% Name graphics window
figure('name','Histogram');
for logNumber = 1:length(logging)
    
    % Remove first element
    first(logNumber) = logging{logNumber}(1);
    logging{logNumber}(1) = [];
    
    % Average
    average(logNumber) = mean(logging{logNumber});
    
    % Remover outliers
    i = 1;
    while i <= length(logging{logNumber})
        if logging{logNumber}(i) >  average(logNumber)*1.25
           logging{logNumber}(i) = [];
        elseif logging{logNumber}(i) <  average(logNumber)*0.75
           logging{logNumber}(i) = [];
        else
            i = i+1;
        end
    end
    
   % Make histogram
   subplot(3,3,logNumber)
   hist(logging{logNumber},20)
   title(name{logNumber})
   
end


% Average chart
hold off;
figure('name','Average');
set(gca,'LooseInset',get(gca,'TightInset'))

%y = [average(1) average(2); average(3) average(4); average(5)  average(6); 0 average(7)];
y = [average(1) average(2); average(3) average(4); average(5)  average(6)];
bar(y)
%set(gca,'XTickLabel',{'C#', 'F# direct', 'F#', 'F# Paralell'})
set(gca,'XTickLabel',{'C#', 'F# direct', 'F#'})
legend('No cache', 'With cache');

ylabel('seconds') % label for y axis

%set(gca,'YScale','log')


% Fist chart
hold off;
figure('name','First');
set(gca,'LooseInset',get(gca,'TightInset'))

y = [first(1) first(2); first(3) first(4); first(5)  first(6); 0 first(7)];
bar(y)
set(gca,'XTickLabel',{'C#', 'F# direct', 'F#', 'F# Paralell'})
legend('No cache', 'With cache');

%set(gca,'YScale','log')

